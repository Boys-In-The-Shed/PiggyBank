FROM mcr.microsoft.com/dotnet/sdk:5.0

ARG TERRAFORM_VERSION=1.0.5

# Install Linux Packages
RUN apt-get update
RUN apt-get install -y zip

# Install Terraform.
RUN wget https://releases.hashicorp.com/terraform/${TERRAFORM_VERSION}/terraform_${TERRAFORM_VERSION}_linux_amd64.zip
RUN unzip terraform_${TERRAFORM_VERSION}_linux_amd64.zip
RUN mv terraform /usr/bin
RUN rm terraform_${TERRAFORM_VERSION}_linux_amd64.zip