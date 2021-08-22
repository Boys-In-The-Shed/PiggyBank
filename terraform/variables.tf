locals {
	hostname = var.environment_name == "production" ? "api.piggybank.lukejoshuapark.io" : "api-${var.environment_name}.piggybank.lukejoshuapark.io"
}

variable "environment_name" {
	type = string
}