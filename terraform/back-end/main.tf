terraform {
	required_providers {
		aws = {
			source  = "hashicorp/aws"
			version = "~> 3.54.0"
		}
	}
}

data "aws_route53_zone" "hosted_zone" {
	name = "lukejoshuapark.io."
}