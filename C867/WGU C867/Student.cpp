#include <iostream>
#include <string>
#include "degree.h"
using namespace std;
#include "student.h"

//Student class function definitions

//Constructor
Student::Student() {
	this->studentID = "";
	this->firstName = "";
	this->lastName = "";
	this->emailAddress = "";
	this->studentAge = 0;
	this->daysToComplete[0] = 0;
	this->daysToComplete[1] = 0;
	this->daysToComplete[2] = 0;
	this->degreeProgram;
}
//Sets a student's ID
void Student::SetID(string studentId) {
	this->studentID = studentId;

	return;
}
//Sets a student's first name
void Student::SetFirstName(string firstName) {
	this->firstName = firstName;

	return;
}
//Sets a student's last name
void Student::SetLastName(string lastName) {
	this->lastName = lastName;

	return;
}
//Sets a student's email address
void Student::SetEmailAddress(string emailAddress) {
	this->emailAddress = emailAddress;

	return;
}
//Sets a student's age
void Student::SetStudentAge(int age) {
	this->studentAge = age;

	return;
}
//Sets the number of days to complete a student's courses
void Student::SetDaystoComplete(int completeTime1, int completeTime2, int completeTime3) {
	this->daysToComplete[0] = completeTime1;
	this->daysToComplete[1] = completeTime2;
	this->daysToComplete[2] = completeTime3;

	return;
}
//Sets a student's degree program
void Student::SetDegreeProgram(degree degreeProgram) {
	this->degreeProgram = degreeProgram;

	return;
}
//Gets a student's ID
string Student::GetStudentID() {
	return studentID;
}
//Gets a student's first name
string Student::GetFirstName() {
	return firstName;
}
//Gets a student's last name
string Student::GetLastName() {
	return lastName;
}
//Gets a student's email address
string Student::GetEmailAddress() {
	return emailAddress;
}
//Gets a student's age
int Student::GetStudentAge() {
	return studentAge;
}
//Get's the days to complete for each of the student's classes
int Student::GetDaysToComplete1() {
	return daysToComplete[0];
}
int Student::GetDaysToComplete2() {
	return daysToComplete[1];
}
int Student::GetDaysToComplete3() {
	return daysToComplete[2];
}
//Gets a student's degree program
degree Student::GetDegreeProgram() {
	return degreeProgram;
}
//Prints a student's ID
void Student::PrintStudentID() {
	cout << studentID;

	return;
}
//Prints a student's first name
void Student::PrintFirstName() {
	cout << firstName << endl;

	return;
}
//Prints a student's last name
void Student::PrintLastName() {
	cout << lastName << endl;

	return;
}
//Prints a student's email address
void Student::PrintEmailAddress() {
	cout << emailAddress << endl;

	return;
}
//Prints a student's age
void Student::PrintStudentAge() {
	cout << studentAge << endl;

	return;
}
//Prints student's days to complete each of their courses
void Student::PrintDaysInCourse() {
	cout << daysToComplete[0] << ", " << daysToComplete[1] << ", " << daysToComplete[2] << endl;

	return;
}
//Prints a student's degree program
void Student::PrintDegreeProgram() {
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
		degreeString = "ERROR";
	}
	cout << degreeString << endl;

	return;
}
//Prints all of the student info in the requested format
void Student::PrintAllStudentInfo() {
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
		degreeString = "ERROR";
	}

	cout << studentID << "   First Name: " << firstName << "   Last Name: " << lastName << "   Age: " << studentAge << "   daysInCourse: {" << daysToComplete[0] << ", " << daysToComplete[1]
		<< ", " << daysToComplete[2] << "}   Degree Program: " << degreeString << "." << endl;

	return;
}