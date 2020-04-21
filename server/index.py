import pika
import uuid
from datetime import datetime
from elasticsearch import Elasticsearch

connection = pika.BlockingConnection(
    pika.ConnectionParameters(host='localhost'))
channel = connection.channel()

channel.queue_declare(queue='hello')

es = Elasticsearch()

def callback(ch, method, properties, body):
    print(" [x] Received %r" % body)
    doc = {
        'author': 'bis',
        'text': body.decode("utf-8"),
        'timestamp': datetime.now(),
    }
    print(" [x] Inserting %r" % body)
    res = es.index(index="test-index", id=uuid.uuid4(), body=doc)
    print(f"result: {res['result']}")


channel.basic_consume(
    queue='hello', on_message_callback=callback, auto_ack=True)

print(' [*] Waiting for messages. To exit press CTRL+C')
channel.start_consuming()
