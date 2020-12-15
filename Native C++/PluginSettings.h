#pragma once

#ifdef PLUGIN_EXPORT // if defined
#define PLUGIN_API __declspec(dllexport)
#elif PLUGIN_IMPORT // if not defined
#define PLUGIN_API __declspec(dllimport)
#else
#define PLUGIN_API
#endif