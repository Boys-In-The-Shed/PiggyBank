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

provider "aws" {
	region = "ap-southeast-2"
	alias  = "back-end"
}

provider "aws" {
	region = "us-east-1"
	alias  = "front-end"
}

module "back-end" {
	source = "./back-end"

	environment_name = var.environment_name

	providers = {
		aws = aws.back-end
	}
}

module "front-end" {
	source = "./front-end"

	environment_name = var.environment_name
	
	providers = {
		aws = aws.front-end
	}
}