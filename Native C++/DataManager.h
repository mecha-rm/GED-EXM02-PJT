#pragma once

#include "PluginSettings.h"

#include <vector>
#include <string>

// struct for data records
struct DataRecord
{
	char* data = nullptr;
	int size = 0;
};

// the data manager class
class PLUGIN_API DataManager
{
public:
	// constructor
	DataManager();

	// destructor - deletes data
	~DataManager();

	// adds a data record
	void AddDataRecord(char* data, int size);

	// adds a data record
	void AddDataRecord(DataRecord& dataRecord);

	// removes a data record via its data 
	DataRecord RemoveDataRecord(char* data, int size);

	// removes a data record via its index
	DataRecord RemoveDataRecord(int index);

	// removes a data record via its object
	// it checks the 'data' variable to see if its already in the list.
	// The data and size must match.
	DataRecord RemoveDataRecord(const DataRecord& dataRecord);

	// clears out all data records. This does not delete the pointer data.
	void ClearAllDataRecords();

	// deletes data record and removes it from the list.
	void DeleteDataRecord(char* data, int size);

	// deletes data record via its index
	void DeleteDataRecord(int index);

	// deletes data record via its object. The data and length must match.
	void DeleteDataRecord(DataRecord& dataRecord);

	// deletes all data records
	void DeleteAllDataRecords();

	// checks to see if a provided data record is in the manager
	bool ContainsDataRecord(char* data, int size) const;

	// checks to see if a provided data record is in the manager
	bool ContainsDataRecord(const DataRecord& dataRecord) const;

	// gets the data at the requested index.
	char* GetData(int index) const;

	// returns the size of the record data
	// returns -1 if invalid index.
	int GetDataSize(int index) const;

	// gets the data record at the provided index.
	// returns empty data record if invalid
	DataRecord GetDataRecord(int index) const;

	// gets the amount of data records
	int GetDataRecordCount() const;

	// checks to see if there are any data records
	bool IsEmpty() const;

	// gets the file tied to this record system.
	const std::string& GetFile() const;

	// sets the file for this file system
	void SetFile(std::string newFile);

	// checks to see if the file is available. This does NOT save the file.
	bool FileAccessible() const;

	// imports the records from the provided file
	bool ImportRecords();

	// exports records to saved file.
	bool ExportRecords();

private:
	// file
	std::string file = "";

	// list of records
	std::vector<DataRecord> dataRecords;

protected:

};

