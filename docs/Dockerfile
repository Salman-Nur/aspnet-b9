FROM ubuntu

ARG DEBIAN_FRONTEND=noninteractive
RUN apt update 
RUN apt install -y apache2

WORKDIR /var
EXPOSE 80
CMD apachectl -D FOREGROUND

