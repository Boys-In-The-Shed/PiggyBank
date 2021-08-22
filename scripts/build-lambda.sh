#!/bin/sh

TOP_LEVEL_DIR=$(pwd)

cd ./src/PiggyBank/PiggyBank.Lambda.Function
dotnet publish -c release || exit 1

cd ./bin/release/netcoreapp3.1/publish
zip pb-lambda-package.zip *

mv pb-lambda-package.zip $TOP_LEVEL_DIR/pb-lambda-package.zip