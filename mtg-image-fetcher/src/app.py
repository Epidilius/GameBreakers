from flask import Flask, jsonify, request
from .scraper import *


app = Flask('mtg-price-fetcher')

@app.route('/cards')
def fetch_card_price():
    multiverse_id = request.args.get('multiverse_id')

    result = get_card_image(multiverse_id)

    return result
