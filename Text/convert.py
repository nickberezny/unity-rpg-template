import sys
import os 
n = len(sys.argv)
filename = sys.argv[1]

def add_line(num, text, pointer):
	json.write(",\n");
	json.write("\t\"" + num + "\":{\n");
	json.write("\t\t\"text\":" +  text.replace("\n", "") + ",\n");
	json.write("\t\t\"pointer\":");
	json.write(str(pointer).replace("\\n","").replace("'", ""))
	json.write(",\n\t\t\"states\":[\"null\"]")
	json.write("\n\t}");


file = open(filename + ".dia", "r")
json = open(os.pardir + "/Assets/Data/Dialogue/" + filename + ".json", "w")


textArray = file.readlines()
file.close()

json.write("{\n\t\"-1\":{\n\t\t\"entryState\":1\n\t}")

for line in textArray:

	if(line == '\n' or line[0] == '#'):
		continue

	lineText = line.split("\t")

	if(lineText[0] == ''):
		num = lineText[1]
		text = lineText[2]
		pointer = lineText[3:len(lineText)]
		add_line(num, text, pointer)

	else:
		num = lineText[0]
		text = lineText[1]
		pointer = lineText[2:len(lineText)]
		add_line(num, text, pointer)

json.write("\n}")
json.close()

