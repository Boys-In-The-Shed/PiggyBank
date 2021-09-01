#!/bin/sh

./scripts/iac-init.sh || exit 1
./scripts/iac-plan.sh || exit 1
./scripts/iac-apply.sh || exit 1