#!/bin/sh

cd terraform
terraform apply --auto-approve .tfplan || exit 1