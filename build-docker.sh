#! /bin/bash

cd ./app/CloudCalculusCalculator
docker build -t calculator-image -f Dockerfile .
docker stop core-calculator || true && docker rm core-calculator || true
docker run -d -p 5000:5000 --name core-calculator calculator-image
start http://localhost:5000