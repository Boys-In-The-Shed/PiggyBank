terraform {
	backend "s3" {
		dynamodb_table       = "piggy-bank-terraform"
		bucket               = "piggy-bank-terraform"
		region               = "ap-southeast-2"
		key                  = ".tfstate"
		workspace_key_prefix = ""
	}

	required_providers {
		aws = {
			source  = "hashicorp/aws"
			version = "~> 3.54.0"
		}
	}
}

provider "aws" {
	region = "ap-southeast-2"
}

data "aws_route53_zone" "hosted_zone" {
	name = "lukejoshuapark.io."
}