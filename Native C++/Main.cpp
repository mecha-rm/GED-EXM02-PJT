/*
* Name: Roderick "R.J." Montague
* Student Number: 100701758
* Date: 12/15/2020
* Description: DLL for storing data.
*   - This plugin will likely be modified and re-used for the GDW project.
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
    std::string file = "test.dat"; // test.dat also works
    dm.SetFile(file);

    // string test
    {
        // this wouldn't work since the data is deleted upon leaving this bracket set.
        // const int LEN = 12;
        // char charArr[LEN] = "string test";

        std::string str = "string test 1";
        char* charArr = new char[str.length()];

        memcpy(charArr, str.c_str(), str.length()); // data must be copied since .c_str() and .data() are const.

        dm.AddDataRecord(charArr, str.length());
    }

    // char array test (uses dynamic variable since static would be deleted upon leaving)
    {
        // the LEN needs to be 1 higher than what you're initializing it with.
        // this is because it's adding a null-termination character at the end.
        const int LEN = 14;
        char* charArr = new char[LEN] {"string test 2"};

        DataRecord dr{ charArr, LEN};
        dm.AddDataRecord(dr);
        dm.RemoveDataRecord(dr);
        dm.AddDataRecord(dr);
    }

    // integer test
    {
        int x = 4;

        char* data = new char[sizeof(int)];
        memcpy(data, &x, sizeof(int));

        // containment check
        std::cout << "Contains Int Record? " << std::boolalpha << dm.ContainsDataRecord(data, sizeof(int)) << std::endl;

        dm.AddDataRecord(data, sizeof(int));
    }

    // float test
    {
        float x = 2.3F;

        char* data = new char[sizeof(x)];
        memcpy(data, &x, sizeof(x));

        DataRecord dr{ data, sizeof(x) };
        dm.AddDataRecord(dr);

        // containment check
        std::cout << "Contains Float Record? " << std::boolalpha << dm.ContainsDataRecord(dr) << std::endl;
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
    dm.ExportDataRecords();
    std::cout << "Export Complete." << std::endl;

    std::cout << "Deleting All Data..." << std::endl;
    dm.DeleteAllDataRecords();
    std::cout << "\nLoading Back in Data..." << std::endl;
    dm.ImportDataRecords();
    std::cout << "Data Import Complete." << std::endl;

    std::cout << "\nPrinting Imported Data:" << std::endl;
    {
        char* data;
        int size;
        std::string str;

        data = dm.GetData(0);
        size = dm.GetDataSize(0);
        str = std::string(data, size);

        std::cout << str << " - size: " << size << std::endl;

        data = dm.GetData(1);
        size = dm.GetDataSize(1);
        str = std::string(data, size);
        std::cout << data << " - size: " << size << std::endl;

        int x = 0;
        data = dm.GetData(2);
        memcpy(&x, data, sizeof(int));
        std::cout << x << " - size: " << dm.GetDataSize(2) << std::endl;

        float y = 0.0F;
        data = dm.GetData(3);
        memcpy(&y, data, sizeof(float));
        std::cout << y << " - size: " << dm.GetDataSize(3) << std::endl;

        double z = 0.0;
        data = dm.GetData(4);
        memcpy(&z, data, sizeof(double));
        std::cout << z << " - size: " << dm.GetDataSize(4) << std::endl;
    }

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
