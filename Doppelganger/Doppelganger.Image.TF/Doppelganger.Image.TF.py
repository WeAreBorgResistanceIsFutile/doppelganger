
# USAGE
# python classify_image.py --image images/soccer_ball.jpg --model vgg16

# import the necessary packages
from keras.applications import ResNet50
from keras.applications import InceptionV3
from keras.applications import Xception # TensorFlow ONLY
from keras.applications import VGG16
from keras.applications import VGG19
from keras.applications import imagenet_utils
from keras.applications.inception_v3 import preprocess_input
from keras.preprocessing.image import img_to_array
from keras.preprocessing.image import load_img
import numpy as np
import argparse
import cv2

# construct the argument parse and parse the arguments
ap = argparse.ArgumentParser()
ap.add_argument("-i", "--image", required=True,
	help="path to the input image")

args = vars(ap.parse_args())

def extract_features(model, image, preprocess):    
    image = img_to_array(image)
    image = np.expand_dims(image, axis=0)
    image = preprocess(image)    
    preds = model.predict(image)    
    return preds.flatten()

def load_image_224(imagePath): 
    inputShape = (224, 224)
    preprocess = imagenet_utils.preprocess_input
    image224 = load_img(imagePath, target_size=inputShape)
    return (image224, preprocess)

def load_image_299(imagePath): 
    inputShape = (299, 299)
    preprocess = preprocess_input
    image = load_img(imagePath, target_size=inputShape)
    return (image, preprocess)

# initialize the input image shape (224x224 pixels) along with
# the pre-processing function (this might need to be changed
# based on which model we use to classify our image)


(image224,preprocess) = load_image_224(args["image"])
(image299,preprocess) = load_image_299(args["image"])

model = VGG16(weights='imagenet', include_top=False, pooling='avg')
features = extract_features(model, image224, preprocess);
print(features)
model = VGG19(weights='imagenet', include_top=False, pooling='avg')
features = extract_features(model, image224, preprocess);
print(features)
model = ResNet50(weights='imagenet', include_top=False, pooling='avg')
features = extract_features(model, image224, preprocess);
print(features)


model = Xception(weights='imagenet', include_top=False, pooling='avg')
features = extract_features(model, image299, preprocess);
print(features)

model = InceptionV3(weights='imagenet', include_top=False, pooling='avg')
features = extract_features(model, image299, preprocess);




