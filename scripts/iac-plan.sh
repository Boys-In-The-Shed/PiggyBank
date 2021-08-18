#!/bin/sh

cd terraform
terraform plan --out=.tfplan || exit 1