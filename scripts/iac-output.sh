#!/bin/sh

cd terraform
CLOUDFRONT_ID=$(terraform output --raw cloudfront_distribution_id) || exit 1
echo "::set-output name=CLOUDFRONT_ID::$CLOUDFRONT_ID"