FROM confluentinc/cp-kafka:6.2.4

## Add the wait script to the image
ADD https://github.com/ufoscout/docker-compose-wait/releases/download/2.9.0/wait /wait
USER root
RUN chmod +x /wait
USER appuser

CMD /wait && /etc/confluent/docker/run & sleep infinity