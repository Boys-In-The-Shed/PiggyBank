resource "aws_lambda_function" "lambda_api" {
	function_name = "pb_api_${var.environment_name}"
	filename      = "${path.module}/../pb-lambda-package.zip"
	role          = aws_iam_role.lambda_api_role.arn
	handler       = "PiggyBank.Lambda.Function::PiggyBank.Lambda.Function.Function::FunctionHandler"
	runtime       = "dotnetcore3.1"

	source_code_hash = filebase64sha256("${path.module}/../pb-lambda-package.zip")
}

resource "aws_iam_role" "lambda_api_role" {
	name               = "pb_api_${var.environment_name}_lambda"
	assume_role_policy = file("${path.module}/lambda_role_assume.json")
}

resource "aws_iam_role_policy_attachment" "lambda_api_role_basic_execution_attachment" {
	role       = aws_iam_role.lambda_api_role.name
	policy_arn = "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole"
}

resource "aws_iam_policy" "lambda_api_role_policy" {
	name   = "apes2d_api"
	policy = file("${path.module}/lambda_role_policy.json")
}

resource "aws_iam_role_policy_attachment" "lambda_api_role_policy_attachment" {
	role       = aws_iam_role.lambda_api_role.name
	policy_arn = aws_iam_policy.lambda_api_role_policy.arn
}