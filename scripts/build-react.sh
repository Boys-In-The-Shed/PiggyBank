#!/bin/sh

cd src/pb-app

if [ ! -d "node_modules" ]; then
	npm install || exit 1
fi

npm run build || exit 1
