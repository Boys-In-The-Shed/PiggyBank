#!/bin/sh

cd terraform
terraform destroy --auto-approve || exit 1