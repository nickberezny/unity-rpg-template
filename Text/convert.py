import sys
n = len(sys.argv)
filename = sys.argv[1]

def add_line(num, text, pointer):
	json.write("{");
	json.write("\"" + num + "\":{");
	json.write("\"text\":\"" +  text.replace("\n", "") + "\",");
	json.write("\"pointer\":");
	json.write(str(pointer).replace("\\n","").replace("'", ""))
	json.write("}");


file = open(filename + ".txt", "r")
json = open(filename + ".json", "w")

textArray = file.readlines()

for line in textArray:
	lineText = line.split("\t")
	if(lineText[0] == ''):
		num = lineText[1]
		text = lineText[2]
		pointer = lineText[3:len(lineText)]
		add_line(num, text, pointer)
		print(pointer)

	else:
		num = lineText[0]
		text = lineText[1]
		pointer = lineText[2:len(lineText)]
		add_line(num, text, pointer)


