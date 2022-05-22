# kafka-producer-minimal-api
A high throughput singleton Kafka Producer example with .NET 6 Minimal APIs

Start app:
```sh
docker-compose up -d
```

Create topic:
```sh
docker exec -it kafkaweb_broker01 kafka-topics --bootstrap-server broker01:9092,broker02:9092,broker03:9092 --topic biz --create --partitions 8 --replication-factor 3
```

List topics:
```sh
docker exec -it kafkaweb_broker01 kafka-topics --bootstrap-server broker01:9092,broker02:9092,broker03:9092 --list
```

Describe the topic:
```sh
docker exec -it kafkaweb_broker01 kafka-topics --bootstrap-server broker01:9092,broker02:9092,broker03:9092 --topic biz --describe
```

Start a consumer:
```sh
docker exec -it kafkaweb_broker01 kafka-console-consumer --topic biz --bootstrap-server broker01:9092,broker02:9092,broker03:9092 --from-beginning --property print.key=true --property key.separator="-" --key-deserializer "org.apache.kafka.common.serialization.LongDeserializer"
```

Start redis client:
```sh
docker exec -it kafkaweb_redis redis-cli
```

- `keys *` shows all keys.
    - `bizCounter` _requests_ counter.
    - `p` persisted requests. 
    - `pp` possibly persisted requests. 
    - `np` not persisted requests. 
- `get key-name` actual value of `key-name`.

Use Apache Bench to send 100k requests in 1k batchs:
```sh
ab -n 100000 -c 1000 -l http://localhost:8888/
```

Example result:
```
This is ApacheBench, Version 2.3 <$Revision: 1843412 $>
Copyright 1996 Adam Twiss, Zeus Technology Ltd, http://www.zeustech.net/
Licensed to The Apache Software Foundation, http://www.apache.org/

Benchmarking localhost (be patient)
Completed 10000 requests
Completed 20000 requests
Completed 30000 requests
Completed 40000 requests
Completed 50000 requests
Completed 60000 requests
Completed 70000 requests
Completed 80000 requests
Completed 90000 requests
Completed 100000 requests
Finished 100000 requests


Server Software:        Kestrel
Server Hostname:        localhost
Server Port:            8888

Document Path:          /
Document Length:        Variable

Concurrency Level:      1000
Time taken for tests:   32.558 seconds
Complete requests:      100000
Failed requests:        0
Total transferred:      26500000 bytes
HTML transferred:       12600000 bytes
Requests per second:    3071.43 [#/sec] (mean)
Time per request:       325.581 [ms] (mean)
Time per request:       0.326 [ms] (mean, across all concurrent requests)
Transfer rate:          794.85 [Kbytes/sec] received

Connection Times (ms)
              min  mean[+/-sd] median   max
Connect:        0   10  13.8      4     115
Processing:    24  301 505.3    186    7621
Waiting:        3  296 505.1    181    7619
Total:         41  311 505.0    195    7621

Percentage of the requests served within a certain time (ms)
  50%    195
  66%    230
  75%    257
  80%    280
  90%    371
  95%   1246
  98%   1387
  99%   1683
 100%   7621 (longest request)
```
