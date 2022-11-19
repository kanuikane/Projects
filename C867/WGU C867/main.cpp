// main.cpp : This file contains the 'main' function. Program execution begins and ends here.

#include <iostream>
#include "student.h"
#include "roster.h"
#include "degree.h"
#include <string>
#include <array>
using namespace std;

int main()
{
    cout << "Course Title: Scripting and Programming - Applications - C867" << endl;
    cout << "Programming Language Used: C++" << endl;
    cout << "Student ID: 001336713 " << endl;
    cout << "Name: Kanuikane Kubli" << endl << endl;


    const string studentData[] = {
        "A1,John,Smith,John1989@gm ail.com,20,30,35,40,SECURITY",
        "A2,Suzan,Erickson,Erickson_1990@gmailcom,19,50,30,40,NETWORK",
        "A3,Jack,Napoli,The_lawyer99yahoo.com,19,20,40,33,SOFTWARE",
        "A4,Erin,Black,Erin.black@comcast.net,22,50,58,40,SECURITY",
        "A5,Kanuikane,Kubli,kkubli1@wgu.edu,29,7,18,32,SOFTWARE"
    };

    //Creates a new class roster
    Roster* classRoster = new Roster(5);

    //Adds an index of student data to a new Student class object
    for (int i = 0; i < 5; ++i) {
        classRoster->add(studentData[i]);
    };

    //Prints the class roster
    classRoster->printAll();

    //Prints all emails found to be invalid
    classRoster->printInvalidEmails();

    for (int i = 0; i < 5; ++i) {

        classRoster->printAverageDaysInCourse(classRoster->GetStudentID(i));
    }
    cout << endl;

    classRoster->printByDegreeProgram(SOFTWARE);
    cout << endl;

    classRoster->remove("A3");
    cout << endl;

    classRoster->printAll();
    cout << endl;

    classRoster->remove("A3");
    cout << endl;

    //Destructor
    classRoster->~Roster();
    delete classRoster;

}