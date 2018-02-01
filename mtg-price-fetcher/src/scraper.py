from flask import Flask, jsonify, request
import requests
import urllib
import json


MTGSTOCKS_BASE_URL = 'https://api.mtgstocks.com/'
MTGSTOCKS_QUERY_ID = 'search/autocomplete/'
MTGSTOCKS_QUERY_DATA = 'prints/'

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

    price = 0
    foilPrice = 0

    try:
        foilPrice = get_foil_price(content)
    except Exception:
        pass

    try:
        price = get_avg_price(content)
    except:
        pass

    return {
        'price_foil': foilPrice,
        'price': price,
    }

def get_foil_price(content):
    return content['latest_price']['foil']
def get_avg_price(content):
    return content['latest_price']['avg']

def card_id_from_set(name, set):
    card_id = card_id_from_name(name)
    query_url = generate_search_url_for_id(card_id)
    response = requests.get(query_url, allow_redirects=False).json()
    
    setName = response['card_set']['name']
    if setName == set:
        return response['id']

    sets = response['sets']

    for card in sets:
        set_name = card['set_name']
        if set_name == set:
            return card['id']

def get_card_price(name, set=None):
    if set is not None:
        card_id = card_id_from_set(name, set)
    else:
        card_id = card_id_from_name(name)

    return jsonify(scrape_price(card_id))


