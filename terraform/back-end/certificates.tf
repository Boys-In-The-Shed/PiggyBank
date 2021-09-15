resource "aws_acm_certificate" "api_certificate" {
	domain_name       = local.hostname
	validation_method = "DNS"
}

resource "aws_acm_certificate_validation" "api_certificate_validation" {
	certificate_arn         = aws_acm_certificate.api_certificate.arn
	validation_record_fqdns = [for r in aws_route53_record.certificate_validation_records : r.fqdn]
}

resource "aws_route53_record" "certificate_validation_records" {
	for_each = {
		for dvo in aws_acm_certificate.api_certificate.domain_validation_options : dvo.domain_name => {
			name   = dvo.resource_record_name
			record = dvo.resource_record_value
			type   = dvo.resource_record_type
		}
	}

	allow_overwrite = true
	name            = each.value.name
	records         = [each.value.record]
	ttl             = 120
	type            = each.value.type
	zone_id         = data.aws_route53_zone.hosted_zone.zone_id
}