#ifndef Student_Header
#define Student_Header
#include <iostream>
#include <string>
#include "degree.h"
using namespace std;

class Student {
public:
	Student();
	//setters
	void SetID(string studentId);
	void SetFirstName(string firstName);
	void SetLastName(string lastName);
	void SetEmailAddress(string studentEmail);
	void SetStudentAge(int studentAge);
	void SetDaystoComplete(int completeTime1, int completeTime2, int completeTime3);
	void SetDegreeProgram(degree degreeProgram);
	//getters
	string GetStudentID();
	string GetFirstName();
	string GetLastName();
	string GetEmailAddress();
	int GetStudentAge();
	int GetDaysToComplete1();
	int GetDaysToComplete2();
	int GetDaysToComplete3();
	degree GetDegreeProgram();
	void PrintStudentID();
	void PrintFirstName();
	void PrintLastName();
	void PrintEmailAddress();
	void PrintStudentAge();
	void PrintDaysInCourse();
	void PrintDegreeProgram();
	void PrintAllStudentInfo();


private:
	string studentID, firstName, lastName, emailAddress;
	int studentAge, daysToComplete[3];
	degree degreeProgram;
};


#endif