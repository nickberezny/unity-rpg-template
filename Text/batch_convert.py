from os import listdir
import sys
import subprocess


files = listdir()

for file in files:
	if(file.split('.')[1] == 'dia'):
		subprocess.call("python convert.py " + file.split('.')[0], shell=True)
		print("Converting " + file)