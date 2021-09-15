locals {
	hostname = var.environment_name == "production" ? "app.piggybank.lukejoshuapark.io" : "app-${var.environment_name}.piggybank.lukejoshuapark.io"
}

variable "environment_name" {
	type = string
}