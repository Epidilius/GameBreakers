from flask import Flask, jsonify, request
import requests
import urllib
import json


MTGSTOCKS_BASE_URL = 'https://api.mtgstocks.com/'
MTGSTOCKS_QUERY_ID = 'search/autocomplete/'
MTGSTOCKS_QUERY_DATA = 'prints/'

SETS_PATH = MTGSTOCKS_BASE_URL + '/sets'

def generate_search_url(name):
    formatted_name = urllib.parse.quote(name)
    return MTGSTOCKS_BASE_URL + MTGSTOCKS_QUERY_ID + formatted_name
def generate_search_url_for_id(id):
    return MTGSTOCKS_BASE_URL + MTGSTOCKS_QUERY_DATA + str(id)

def card_id_from_name(name):
    query_url = generate_search_url(name)
    response = requests.get(query_url, allow_redirects=False)
    cardJSON = response.json()

    if (response.status_code in range(301, 307)):
        return response.headers['Location']
    elif (response.status_code == requests.codes.ok):
        return cardJSON[0]['id']

    return None

def scrape_price(id):
    query_url = generate_search_url_for_id(id)
    response = requests.get(query_url, allow_redirects=False)
    content = response.json()

    return {
        'price_foil': content['latest_price']['foil'],
        'price': content['latest_price']['avg'],
    }

def card_id_from_set(name, card_set):
    card_id = card_id_from_name(name)
    query_url = generate_search_url_for_id(card_id)
    response = requests.get(query_url, allow_redirects=False).json()
    
    if response['card_set']['name'] == card_set:
        return response['id']

    sets = response['sets']

    for card in sets:
        if card['set_name'] == card_set:
            return card['id']

def get_card_price(name, card_set=None):
    if card_set is not None:
        card_id = card_id_from_set(name, card_set)
    else:
        card_id = card_id_from_name(name)

    return jsonify(scrape_price(card_id))


