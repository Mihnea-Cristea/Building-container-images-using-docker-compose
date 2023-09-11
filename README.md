# Building-container-images-using-docker-compose
The Pet Shop web application is built on a development environment on a machine, deployed locally, using Docker Compose, instead of a simple Dockerfile

How to run :

Verify the docker-compose.yml , docker-compose-build.yml and Dockerfile files to check if the configuration setup is a match between themselves, then open up a Powershell console to start building :

A simpler way of checking that is checking the merged configuration files running the docker compose config command :

docker compose -f ./docker-compose.yml -f ./docker-compose-build.yml config

docker-compose-build.yaml will override any matchy configuration settings from the docker-compose.yml file

docker compose -f ./docker-compose.yml -f ./docker-compose-build.yml build

The images will be built with the 'build' command, and they will be tagged as "dev" as specified in the docker-compose-build file

docker compose -f ./docker-compose.yml -f ./docker-compose-dev.yml config

This checks the merged build configuration using the local image tags rather than the published Docker Hub images

Now, run the app if everything has been built correctly:

docker compose -f ./docker-compose.yml -f ./docker-compose-dev.yml up -d

After running, check if all the containers have been built for the specified images:

docker ps

The app should be running correctly, we can check the logs to see if the pipeline is working:

docker compose logs -f
