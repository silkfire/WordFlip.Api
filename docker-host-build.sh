   docker build --build-arg APPSETTINGS=DockerHost --build-arg ENV=Production -f Dockerfile-api -t silkfire/word-flip-api:hostdb-1.0 . \
&& docker image prune -f > /dev/null