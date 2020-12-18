#pragma once

#include "DataManager.h"

#ifdef __cplusplus
extern "C" // convert to C code.
{
#endif
	// adds a data record
	PLUGIN_API void AddDataRecord(char* data, int size);

	// inserts the record
	PLUGIN_API void InsertDataRecord(int index, char* data, int size);

	// removes a data record via its data 
	PLUGIN_API void RemoveDataRecord(char* data, int size);

	// removes a data record via its index
	PLUGIN_API void RemoveDataRecordByIndex(int index);

	// clears out all data records. This does not delete the pointer data.
	PLUGIN_API void ClearAllDataRecords();

	// deletes data record and removes it from the list.
	PLUGIN_API void DeleteDataRecord(char* data, int size);

	// deletes data record via its index
	PLUGIN_API void DeleteDataRecordByIndex(int index);

	// deletes all data records
	PLUGIN_API void DeleteAllDataRecords();

	// checks to see if a provided data record is in the manager
	// maybe make this const like the DataManager function this plugin function is calling?
	PLUGIN_API int ContainsDataRecord(char* data, int size);

	// gets the data at the requested index.
	// maybe find a way to return an array instead of fill an array
	PLUGIN_API void GetData(int index, char* arr, int size);

	// returns the size of the record data
	// returns -1 if invalid index.
	PLUGIN_API int GetDataSize(int index);

	// edits a data record's data, replacing it with newData.
	PLUGIN_API void EditData(int index, char* newData);

	// edits the data record's size
	PLUGIN_API void EditDataSize(int index, int newSize);

	// edits a data record, providing it with new values. This does not delete the existing data from memory.
	PLUGIN_API void EditDataRecord(int index, char* newData, int newSize);

	// gets the amount of data records
	PLUGIN_API int GetDataRecordCount();

	// checks to see if there are any data records
	PLUGIN_API int HasDataRecords();

	// gets the file tied to this record system.
	PLUGIN_API const char* GetFile();

	// sets the file for this file system
	PLUGIN_API void SetFile(const char* newFile);

	// checks to see if the file is available. This does NOT save the file.
	PLUGIN_API int FileAccessible();

	// imports the records from the provided file
	PLUGIN_API int ImportDataRecords();

	// exports records to saved file.
	PLUGIN_API int ExportDataRecords();

#ifdef __cplusplus
}
#endif

