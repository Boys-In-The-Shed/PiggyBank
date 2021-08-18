# Modify the arguments below to update software versions.

ARG UBUNTU_VERSION=21.04

# Start with Ubuntu.
FROM ubuntu:${UBUNTU_VERSION}

ARG TERRAFORM_VERSION=1.0.5

# Install some needed packages.
RUN apt-get update
RUN apt-get install -y git unzip wget xz-utils

# Install Terraform.
RUN wget https://releases.hashicorp.com/terraform/${TERRAFORM_VERSION}/terraform_${TERRAFORM_VERSION}_linux_amd64.zip
RUN unzip terraform_${TERRAFORM_VERSION}_linux_amd64.zip
RUN mv terraform /usr/bin
RUN rm terraform_${TERRAFORM_VERSION}_linux_amd64.zip
