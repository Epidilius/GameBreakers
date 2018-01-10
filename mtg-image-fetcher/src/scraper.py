from flask import Flask, jsonify, request
import requests
import urllib
import json


GATHERER_BASE_URL = 'http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid='
GATHERER_POST_URL =  '&type=card'
IMAGE_DIRECTORY = 'C:\\GameBreakersInventory\\Images\\'
IMAGE_TYPE = '.jpg'

def generate_search_url(multiverse_id):
    return GATHERER_BASE_URL + multiverse_id + GATHERER_POST_URL
def generate_file_location(multiverse_id):
    return IMAGE_DIRECTORY + multiverse_id + IMAGE_TYPE

def get_card_image(multiverse_id):
    query_url = generate_search_url(multiverse_id)
    file_name = generate_file_location(multiverse_id)
    urllib.request.urlretrieve(query_url, file_name)

    return file_name


