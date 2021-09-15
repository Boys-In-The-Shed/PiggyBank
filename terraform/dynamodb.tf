resource "aws_dynamodb_table" "dynamodb_table" {
	name         = "piggybank_table_${var.environment_name}"
	billing_mode = "PAY_PER_REQUEST"
	hash_key     = "PK"

	attribute {
		name = "PK"
		type = "S"
	}
}