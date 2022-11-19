#include <iostream>
#include <string>
#include "degree.h"
#include "student.h"
#include "roster.h"
#include <array>
#include <string>
using namespace std;

//Roster Class

//Class Constructor
Roster::Roster(int classSize) {
	this->classSize = classSize;
	this->index = 0;
	for (int i = 0; i < classSize; ++i) {
		this->classRosterArray[i] = new Student;
	}
	return;
}
//Gets the student ID from the Student class
string Roster::GetStudentID(int index) {

	string studentID = classRosterArray[index]->GetStudentID();
	return studentID;
}
//Creating the new Student objects in the classRosterArray
void Roster::add(string studentData) {

	string studentId, firstName, lastName, emailAddress;
	int studentAge, completeTime1, completeTime2, completeTime3;

	if (index < classSize) {

		classRosterArray[index] = new Student();

		int i = studentData.find(",");
		studentId = studentData.substr(0, i);
		classRosterArray[index]->SetID(studentId);

		int j = i + 1;
		i = studentData.find(",", j);
		firstName = studentData.substr(j, i - j);
		classRosterArray[index]->SetFirstName(firstName);

		j = i + 1;
		i = studentData.find(",", j);
		lastName = studentData.substr(j, i - j);
		classRosterArray[index]->SetLastName(lastName);

		j = i + 1;
		i = studentData.find(",", j);
		emailAddress = studentData.substr(j, i - j);
		classRosterArray[index]->SetEmailAddress(emailAddress);

		j = i + 1;
		i = studentData.find(",", j);
		studentAge = stoi(studentData.substr(j, i - j));
		classRosterArray[index]->SetStudentAge(studentAge);

		j = i + 1;
		i = studentData.find(",", j);
		completeTime1 = stoi(studentData.substr(j, i - j));

		j = i + 1;
		i = studentData.find(",", j);
		completeTime2 = stoi(studentData.substr(j, i - j));

		j = i + 1;
		i = studentData.find(",", j);
		completeTime3 = stoi(studentData.substr(j, i - j));
		classRosterArray[index]->SetDaystoComplete(completeTime1, completeTime2, completeTime3);

		j = i + 1;
		i = studentData.find(",", j);
		string type = studentData.substr(j, i - j);
		if (type == "SECURITY") {
			classRosterArray[index]->SetDegreeProgram(SECURITY);
		}
		else if (type == "NETWORK") {
			classRosterArray[index]->SetDegreeProgram(NETWORK);
		}
		else if (type == "SOFTWARE") {
			classRosterArray[index]->SetDegreeProgram(SOFTWARE);
		}
		else {
			cout << "Degree Not Found." << endl;
		}
		++index;
	}
	return;
}
//Removes a student using their id from the roster
void Roster::remove(string studentId) {

	bool studentFound = false;
	for (int i = 0; i < classSize; ++i) {
		if (classRosterArray[i] == nullptr) {
			continue;
		}
		else if (studentId == classRosterArray[i]->GetStudentID()) {
			classRosterArray[i] = nullptr;
			studentFound = true;
			break;
		}
	}
	if (studentFound == false) {
		cout << "Error: Student " << studentId << " Not Found." << endl;
	}
	else if (studentFound == true) {
		cout << "Student " << studentId << " removed." << endl;
	}
	return;
}
//Print all current students within the roster array
void Roster::printAll() {
	cout << "All current students: " << endl;
	for (int i = 0; i < classSize; ++i) {
		if (classRosterArray[i] == nullptr)
		{
			continue;
		}
		else {
			classRosterArray[i]->PrintAllStudentInfo();
		}
	}
	cout << endl;
	return;
}
//Prints the average number of days for a students courses
void Roster::printAverageDaysInCourse(string studentId) {
	for (int i = 0; i < classSize; ++i) {
		if (studentId == classRosterArray[i]->GetStudentID()) {
			int temparray[3] = { classRosterArray[i]->GetDaysToComplete1(), classRosterArray[i]->GetDaysToComplete2(), classRosterArray[i]->GetDaysToComplete3() };
			double averageDays = (static_cast<double>(temparray[0]) + static_cast<double>(temparray[1]) + static_cast<double>(temparray[2])) / 3.0;
			cout << studentId << "'s Average Days In Their Courses: " << averageDays << endl;;
		}
	}
	return;
}
//Prints emails found to be invalid
void Roster::printInvalidEmails() {
	for (int i = 0; i < classSize; ++i) {
		string emailAddress = classRosterArray[i]->GetEmailAddress();
		if ((emailAddress.find(' ') != string::npos) || (emailAddress.find('.') == string::npos) || (emailAddress.find('@') == string::npos)) {
			cout << classRosterArray[i]->GetStudentID() << "'s email address " << emailAddress << " is invalid." << endl;
		}
	}
	cout << endl;
	return;
}
//Prints all students with specified degree programs
void Roster::printByDegreeProgram(degree degreeProgram) {
	string degreeString;
	if (degreeProgram == SECURITY) {
		degreeString = "SECURITY";
	}
	else if (degreeProgram == NETWORK) {
		degreeString = "NETWORK";
	}
	else if (degreeProgram == SOFTWARE) {
		degreeString = "SOFTWARE";
	}
	else {
		degreeString = "WRONG";
	}
	cout << "Students with degree program: " << degreeString << endl;
	int numStudents = 0;
	for (int i = 0; i < classSize; ++i) {
		if (classRosterArray[i]->GetDegreeProgram() == degreeProgram) {
			classRosterArray[i]->PrintAllStudentInfo();
			++numStudents;
		}
	}
	if (numStudents == 0) {
		cout << "No students with this degree found." << endl;
	}
	return;
}
//Destructor
Roster::~Roster() {

	return;
}