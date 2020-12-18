#include "DataManager.h"

#include <fstream>
#include <sstream>

// constructor
DataManager::DataManager()
{
}

// destructor - deletes all data
DataManager::~DataManager()
{
	// deletes data in records
	DeleteAllDataRecords();
}

// adds a data record
void DataManager::AddDataRecord(char* data, int size)
{
	dataRecords.push_back(DataRecord{ data, size });
}

// adds a data record
void DataManager::AddDataRecord(DataRecord& dataRecord)
{
	dataRecords.push_back(dataRecord);
}

// removes a record by its data and size
DataRecord DataManager::RemoveDataRecord(char* data, int size)
{
	// index of data records
	int index = -1;

	// checks for record
	for (int i = 0; i < dataRecords.size(); i++)
	{
		// if the record has been found
		if (dataRecords[i].data == data && dataRecords[i].size == size)
		{
			index = i;
			break;
		}
	}

	// if the index has been found
	if (index >= 0)
	{
		DataRecord rec = dataRecords[index]; // gets record
		dataRecords.erase(dataRecords.begin() + index); // removes it
		return rec; // returns it

	}
	else // returns empty object
	{
		return DataRecord();
	}
}

// removes data record
DataRecord DataManager::RemoveDataRecord(int index)
{
	// index is valid
	if (index >= 0 && index < dataRecords.size())
	{
		DataRecord rec = dataRecords[index]; // gets record
		dataRecords.erase(dataRecords.begin() + index); // removes it
		return rec; // returns it

	}
	else // returns empty object
	{
		return DataRecord();
	}
}

// checks for data record
DataRecord DataManager::RemoveDataRecord(const DataRecord& dataRecord)
{
	return RemoveDataRecord(dataRecord.data, dataRecord.size);
}

// clear all records
void DataManager::ClearAllDataRecords()
{
	dataRecords.clear();
}

// deletes a data record based on its values
void DataManager::DeleteDataRecord(char* data, int size)
{
	DataRecord rec = RemoveDataRecord(data, size);
	
	// if true, the data was found and removed.
	if (rec.data == data && rec.size == size)
	{
		delete[] rec.data;
	}
}

// deletes record by its index
void DataManager::DeleteDataRecord(int index)
{
	// if the index is invalid, it just deletes a nullptr
	DataRecord rec = RemoveDataRecord(index);
	delete[] rec.data;
}

// deletes the data record
void DataManager::DeleteDataRecord(DataRecord& dataRecord)
{
	return DeleteDataRecord(dataRecord.data, dataRecord.size);
}

// deletes all records
void DataManager::DeleteAllDataRecords()
{
	// deletes all data in the list, which is pointer data
	for (DataRecord rec : dataRecords)
		delete[] rec.data;

	// clears list
	dataRecords.clear();
}

// checks to see if a data record is in the list
bool DataManager::ContainsDataRecord(char* data, int size) const
{
	// checks for data record
	for (DataRecord rec : dataRecords)
	{
		if (rec.data == data && rec.size == size)
			return true;
	}

	return false;
}

// checks for data record
bool DataManager::ContainsDataRecord(const DataRecord& dataRecord) const
{
	return ContainsDataRecord(dataRecord.data, dataRecord.size);
}

// data in manager
char* DataManager::GetData(int index) const
{
	if (index >= 0 && index < dataRecords.size())
		return dataRecords[index].data;
	else
		return nullptr;
}

// gets the data size
int DataManager::GetDataSize(int index) const
{
	// index validity check
	if (index >= 0 && index < dataRecords.size())
		return dataRecords[index].size;
	else
		return -1;
}

// gets data record
DataRecord DataManager::GetDataRecord(int index) const
{
	// index validity check
	if (index >= 0 && index < dataRecords.size())
		return dataRecords[index];
	else
		return DataRecord();
}

// gets amount of data records
int DataManager::GetDataRecordCount() const
{
	return dataRecords.size();
}

// checks to see if the data manager is empty
bool DataManager::HasDataRecords() const
{
	return dataRecords.empty();
}

// gets the file for the data manager
const std::string& DataManager::GetFile() const
{
	return file;
}

// sets the file for the data manager
void DataManager::SetFile(std::string newFile)
{
	file = newFile;
}

// checks to see if the file is accessible
bool DataManager::FileAccessible() const
{
	std::ifstream fs(file, std::ios::in); // opens file for reading
	bool accessible; // checks to see if the file is accessible.

	// if !file is true, then the file couldn't be opened.
	accessible = !fs;
	fs.close();

	// returns the opposite of 'accessible' since it's showing if the file is accessible.
	return !accessible;
}

// imports records
bool DataManager::ImportDataRecords()
{
	// checks to see if the file can be accessed.
	if (!FileAccessible())
		return false;

	// file, and the line from the file
	std::ifstream fileStream(file);
	std::vector<DataRecord> imports; // temporary vector for data manager
	
	// if the file isn't open
	if (!fileStream.is_open())
		return false;

	// reads all records
	/*
	* The records are split into two portions: the record sizes, and the records themselves.
	* The first portion lists the size of each record, which is extracted first.
	* The second portion is the data itself, which is broken up using the sizes from the first portion.
	* A dividing string is used to seperate these portions.
	*/

	// part 1 - gets sizes
	{
		std::string line = "";

		// gets all sizes
		while (std::getline(fileStream, line))
		{
			int tellg = fileStream.tellg();

			// seperator reached
			if (line == SEPERATOR_STR)
				break;

			// the new data record
			DataRecord newRecord;

			// gets the data size as an integer
			std::stringstream ss; // the string stream.
			ss.str(line); // puts line in stream
			ss >> newRecord.size; // gets it as integer

			imports.push_back(newRecord);
		}
	}

	// seperator reached - pointer must be moved over by 1.
	// in right spot
	// char* test = new char[13];
	// fileStream.read(test, 13);

	// int tellg = fileStream.tellg();
	// fileStream.seekg(sizeof(char), fileStream.cur);
	// tellg = fileStream.tellg();

	// now it gets the data from the file.
	
	// part 2 - gets all sizes
	{
		int index = 0;

		// while not at the end of the file, and having records to get data from
		while (!fileStream.eof() && index < imports.size())
		{
			// gets the record, and reads the data
			DataRecord& dr = imports[index];
			dr.data = new char[dr.size];
			fileStream.read(dr.data, dr.size);
			

			index++;
		}
	}

	// closes file
	fileStream.close();

	// combines vectors to perserve existing data
	{
		size_t oldSize = dataRecords.size();
		dataRecords.reserve(oldSize + imports.size()); // allocate memory space
		dataRecords.insert(dataRecords.begin() + oldSize, imports.begin(), imports.end()); // insert data
	}

	return true;
}

// exports the records
bool DataManager::ExportDataRecords()
{
	// file stream
	std::ofstream fileStream;

	// opens the file and clears out existing content in it.
	fileStream.open(file, std::ios::out | std::ios::trunc);

	// if the file isn't open, return false.
	if (!fileStream.is_open())
		return false;

	// if there are no values in the data records vector.
	if (dataRecords.empty())
		return false;


	// writes all records
	/*
	* The records are split into two portions: the record sizes, and the records themselves.
	* The first portion lists the size of each record, which will be used for extracting them later.
	* The second portion is the data itself, which is broken up according to the first portion.
	* A dividing string is used to seperate these portions.
	*/

	// record writing part 1 - data sizes
	for (DataRecord& dr : dataRecords)
	{
		fileStream << std::to_string(dr.size) << "\n";
	}

	// seperator
	fileStream << SEPERATOR_STR << "\n";
	// {
	// 	// puts in the seperator character, removing the null-termination character
	// 	char* arr = new char[SEPERATOR_STR.length() + 1];
	// 	memcpy(arr, SEPERATOR_STR.data(), SEPERATOR_STR.length());
	// 	arr[SEPERATOR_STR.length()] = '\n';
	// 	fileStream << arr;
	// 	delete[] arr;
	// }

	// record writing part 2 - data
	for (DataRecord& dr : dataRecords)
	{
		// prevents garbage data from being written
		// fileStream << dr.data;
		fileStream.write(dr.data, dr.size);
	}

	// closes the file
	fileStream.close();

	return true;
}
