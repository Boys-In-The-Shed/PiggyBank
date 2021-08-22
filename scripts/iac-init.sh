#!/bin/sh

cd terraform
terraform init || exit 1
terraform workspace select $TF_VAR_environment_name || terraform workspace new $TF_VAR_environment_name || exit 1