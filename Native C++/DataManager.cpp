#include "DataManager.h"

#include <fstream>

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
bool DataManager::IsEmpty() const
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

bool DataManager::ImportRecords()
{
	return false;
}

// exports the records
bool DataManager::ExportRecords()
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

	// record writing part 2 - data sizes
	for (DataRecord& dr : dataRecords)
	{
		fileStream << dr.data << "\n";
	}

	// closes the file
	fileStream.close();

	return true;
}
