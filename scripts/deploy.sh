#!/bin/sh

./scripts/build-lambda.sh || exit 1
./scripts/deploy-only.sh || exit 1