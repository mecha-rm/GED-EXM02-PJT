#include "Wrapper.h"

DataManager dm;

// adds a record
PLUGIN_API void AddDataRecord(char* data, int size)
{
	dm.AddDataRecord(data, size);
}

// inserts a record
PLUGIN_API void InsertDataRecord(int index, char* data, int size)
{
	dm.InsertDataRecord(index, data, size);
}

// removes record
PLUGIN_API void RemoveDataRecord(char* data, int size)
{
	// this returns the record, but since this is for a plugin, this function returns void.
	dm.RemoveDataRecord(data, size);
}

// removes a data record via its index.
PLUGIN_API void RemoveDataRecordByIndex(int index)
{
	dm.RemoveDataRecord(index);
}

// clears all records
PLUGIN_API void ClearAllDataRecords()
{
	dm.ClearAllDataRecords();
}

// deletes data record
PLUGIN_API void DeleteDataRecord(char* data, int size)
{
	dm.DeleteDataRecord(data, size);
}

// delete data record by index
PLUGIN_API void DeleteDataRecordByIndex(int index)
{
	dm.DeleteDataRecord(index);
}

// deletes all data records
PLUGIN_API void DeleteAllDataRecords()
{
	dm.DeleteAllDataRecords();
}

// contains data record
PLUGIN_API int ContainsDataRecord(char* data, int size)
{
	return (int)dm.ContainsDataRecord(data, size);
}

// gets the data
PLUGIN_API void GetData(int index, char* arr, int size)
{
	DataRecord dr; // requqested record

	// index bounds check
	if (index >= 0 && index < dm.GetDataRecordCount())
		dr = dm.GetDataRecord(index); // gets the data record
	else
		return;

	// adds in values while there is still space in the providied array.
	for (int i = 0; i < size && i < dr.size; i++)
		arr[i] = dr.data[i]; // enters data
}

// gets the data size
PLUGIN_API int GetDataSize(int index)
{
	return dm.GetDataSize(index);
}

// edits the record data
PLUGIN_API void EditData(int index, char* newData)
{
	dm.EditDataRecord(index, newData);
}

// edits the size of a data record
PLUGIN_API void EditDataSize(int index, int newSize)
{
	return dm.EditDataRecord(index, newSize);
}

// edits data record data and size
PLUGIN_API void EditDataRecord(int index, char* newData, int newSize)
{
	dm.EditDataRecord(index, newData, newSize);
}

// gets the record count
PLUGIN_API int GetDataRecordCount()
{
	return dm.GetDataRecordCount();
}

// checks if the data manager has data records
PLUGIN_API int HasDataRecords()
{
	return (int)dm.HasDataRecords();
}

// gets the file
PLUGIN_API const char* GetFile()
{
	return dm.GetFile().c_str();
}

// sets the file
PLUGIN_API void SetFile(const char* newFile)
{
	return dm.SetFile(std::string(newFile));
}

// checks to see if the file is accessible.
PLUGIN_API int FileAccessible()
{
	return (int)dm.FileAccessible();
}

// imports data records
PLUGIN_API int ImportDataRecords()
{
	return (int)dm.ImportDataRecords();
}

// exports data records
PLUGIN_API int ExportDataRecords()
{
	return (int)dm.ExportDataRecords();
}
