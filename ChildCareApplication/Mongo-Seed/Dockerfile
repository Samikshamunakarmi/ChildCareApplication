FROM mongo:latest

COPY *.json /data/

CMD \
  mongoimport --host Mongo-Seed --db childcare --collection userdetail --type json --file /data/userdetail.json --jsonArray
