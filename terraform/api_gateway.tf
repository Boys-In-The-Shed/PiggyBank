resource "aws_apigatewayv2_api" "api_gateway" {
	name          = "api_gateway_${var.environment_name}"
	protocol_type = "HTTP"

	cors_configuration {
		allow_headers = ["*"]
		allow_methods = ["*"]
		allow_origins = ["*"]
	}
}

resource "aws_cloudwatch_log_group" "api_gateway_logs" {
	name = "api_gateway_${var.environment_name}"
}

resource "aws_apigatewayv2_stage" "api_gateway_stage" {
	api_id      = aws_apigatewayv2_api.api_gateway.id
	name        = "$default"
	auto_deploy = true

	access_log_settings {
		destination_arn = aws_cloudwatch_log_group.api_gateway_logs.arn
		format          = "$context.identity.sourceIp - - [$context.requestTime] $context.routeKey $context.protocol $context.status $context.responseLength $context.requestId"
	}
}

resource "aws_apigatewayv2_integration" "api_gateway_integration" {
	api_id               = aws_apigatewayv2_api.api_gateway.id
	integration_type     = "AWS_PROXY"
	connection_type      = "INTERNET"
	description          = "lambda"
	integration_method   = "POST"
	integration_uri      = aws_lambda_function.lambda_api.invoke_arn
	passthrough_behavior = "WHEN_NO_MATCH"
}

resource "aws_apigatewayv2_route" "api_gateway_routes" {
	for_each = toset([ for x in jsondecode(file("${path.root}/../src/PiggyBank/routes.json")) : "${x.method} ${x.path}" ])

	api_id    = aws_apigatewayv2_api.api_gateway.id
	route_key = each.value
	target    = "integrations/${aws_apigatewayv2_integration.api_gateway_integration.id}"
}

resource "aws_lambda_permission" "lambda_api_permission" {
	statement_id  = "AllowExecutionFromAPIGateway"
	action        = "lambda:InvokeFunction"
	function_name = aws_lambda_function.lambda_api.function_name
	principal     = "apigateway.amazonaws.com"
	source_arn    = "${aws_apigatewayv2_api.api_gateway.execution_arn}/*/*"
}

resource "aws_apigatewayv2_domain_name" "api_gateway_domain_name" {
	domain_name = aws_acm_certificate.api_certificate.domain_name

	domain_name_configuration {
		certificate_arn = aws_acm_certificate.api_certificate.arn
		endpoint_type   = "REGIONAL"
		security_policy = "TLS_1_2"
	}

	depends_on = [
		aws_acm_certificate_validation.api_certificate_validation
	]
}

resource "aws_apigatewayv2_api_mapping" "api_gateway_mapping" {
	api_id      = aws_apigatewayv2_api.api_gateway.id
	domain_name = aws_apigatewayv2_domain_name.api_gateway_domain_name.id
	stage       = aws_apigatewayv2_stage.api_gateway_stage.id
}

resource "aws_route53_record" "api_record" {
	name    = aws_apigatewayv2_domain_name.api_gateway_domain_name.domain_name
	type    = "A"
	zone_id = data.aws_route53_zone.hosted_zone.zone_id

	alias {
		name                   = aws_apigatewayv2_domain_name.api_gateway_domain_name.domain_name_configuration[0].target_domain_name
		zone_id                = aws_apigatewayv2_domain_name.api_gateway_domain_name.domain_name_configuration[0].hosted_zone_id
		evaluate_target_health = false
	}
}