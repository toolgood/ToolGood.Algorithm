// DateTimes.h - 对应 csharp/.../Internals/Functions/DateTimes/
#pragma once

#include <vector>

#include "../../FunctionBase.h"

namespace toolgood {

Function NewFunction_ADDDAYS(std::vector<Function> args);
Function NewFunction_ADDHOURS(std::vector<Function> args);
Function NewFunction_ADDMINUTES(std::vector<Function> args);
Function NewFunction_ADDMONTHS(std::vector<Function> args);
Function NewFunction_ADDSECONDS(std::vector<Function> args);
Function NewFunction_ADDYEARS(std::vector<Function> args);
Function NewFunction_DATE(std::vector<Function> args);
Function NewFunction_DATEDIF(std::vector<Function> args);
Function NewFunction_DATEVALUE(std::vector<Function> args);
Function NewFunction_DAY(std::vector<Function> args);
Function NewFunction_DAYS(std::vector<Function> args);
Function NewFunction_DAYS360(std::vector<Function> args);
Function NewFunction_EDATE(std::vector<Function> args);
Function NewFunction_EOMONTH(std::vector<Function> args);
Function NewFunction_HOUR(std::vector<Function> args);
Function NewFunction_MINUTE(std::vector<Function> args);
Function NewFunction_MONTH(std::vector<Function> args);
Function NewFunction_NETWORKDAYS(std::vector<Function> args);
Function NewFunction_NOW(std::vector<Function> args);
Function NewFunction_SECOND(std::vector<Function> args);
Function NewFunction_TIME(std::vector<Function> args);
Function NewFunction_TIMESTAMP(std::vector<Function> args);
Function NewFunction_TIMEVALUE(std::vector<Function> args);
Function NewFunction_TODAY(std::vector<Function> args);
Function NewFunction_WEEKDAY(std::vector<Function> args);
Function NewFunction_WEEKNUM(std::vector<Function> args);
Function NewFunction_WORKDAY(std::vector<Function> args);
Function NewFunction_YEAR(std::vector<Function> args);
Function NewFunction_YEARFRAC(std::vector<Function> args);

/// 注册 DateTimes 组全部函数。
void Registerdatetimes();

}  // namespace toolgood
