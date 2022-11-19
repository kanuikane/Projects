#ifndef Roster_Header
#define Roster_Header
#include <iostream>
#include <string>
#include "degree.h"
#include <array>

using namespace std;

//Roster class header file and declarations

class Roster {
public:

	Roster(int classSize);
	string GetStudentID(int index);
	void add(string studentData);
	void remove(string studentID);
	void printAll();
	void printAverageDaysInCourse(string studentID);
	void printInvalidEmails();
	void printByDegreeProgram(degree degreeProgram);
	~Roster();
	int classSize;
	int index;
private:
	Student* classRosterArray[5];

};

#endif