#!/bin/sh
apt-get update
apt-get install -y default-jre
java -jar Lavalink.jar &
./PizzaBotGG.App &
tail -f /dev/null