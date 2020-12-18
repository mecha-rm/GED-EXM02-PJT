/*
* Name: Roderick "R.J." Montague
* Student Number: 100701758
* Date: 12/15/2020
* Description: DLL for storing data.
*/
// Main.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include "DataManager.h"
#include <iostream>

void PrintAllDataRecords(const DataManager& dm)
{
    int count = dm.GetDataRecordCount();

    for (int i = 0; i < count; i++)
        std::cout << dm.GetDataSize(i) << std::endl;
}

int main()
{
    std::cout << "Data Management Test\n" << std::endl;

    DataManager dm;
    std::string file = "test.txt";
    dm.SetFile(file);

    // string test
    {
        // this wouldn't work since the data is deleted upon leaving this bracket set.
        // const int LEN = 12;
        // char charArr[LEN] = "string test";

        std::string str = "string test 1";
        char* charArr = new char[str.length()];

        memcpy(charArr, str.c_str(), str.length()); // data must be copied

        dm.AddDataRecord(charArr, str.length());
    }

    // char array test (uses dynamic variable since static would be deleted upon leaving)
    {
        const int LEN = 14;
        char* charArr = new char[LEN] {"string test 2"};

        DataRecord dr{ charArr, LEN};
        dm.AddDataRecord(dr);
    }

    // integer test
    {
        int x = 4;

        char* data = new char[sizeof(int)];
        memcpy(data, &x, sizeof(int));

        dm.AddDataRecord(data, sizeof(int));
    }

    // float test
    {
        float x = 2.3F;

        char* data = new char[sizeof(x)];
        memcpy(data, &x, sizeof(x));

        DataRecord dr{ data, sizeof(x) };
        dm.AddDataRecord(dr);
    }

    // double test
    {
        double x = -21.4;

        char* data = new char[sizeof(x)];
        memcpy(data, &x, sizeof(x));

        dm.AddDataRecord(data, sizeof(x));
    }

    std::cout << "Printing Data Sizes\n";
    PrintAllDataRecords(dm);
    std::cout << std::endl;

    std::cout << "Exporting Records..." << std::endl;
    dm.ExportRecords();
    std::cout << "Export Complete." << std::endl;
    system("pause");
}

// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
