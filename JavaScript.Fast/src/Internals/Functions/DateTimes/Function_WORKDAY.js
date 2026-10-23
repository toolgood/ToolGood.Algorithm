import { Function_N } from '../Function_N.js';
import { MyDate } from '../../MyDate.js';
import { Operand } from '../../../Operand.js';


class Function_WORKDAY extends Function_N {
    get Name() {
        return "Workday";
    }

    constructor(z) {
        super(z);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getDate(engine, tempParameter, 0);
        if (args1.IsError) { return args1; }

        let args2 = this.getNumber(engine, tempParameter, 1);
        if (args2.IsError) { return args2; }

        let startMyDate = new Date(args1.DateValue.ToDateTime().getTime());
        let days = args2.IntValue;
        let list = new Set();
        for (let i = 2; i < this.z.length; i++) {
            let ar = this.getDate(engine, tempParameter, i);
            if (ar.IsError) { return ar; }
            // 将日期转换为YYYY-MM-DD格式以确保Set能够正确比较
            let dateStr = ar.DateValue.ToDateTime().toISOString().split('T')[0];
            list.add(dateStr);
        }

        // 判断是否周末
        let isWeekend = (d) => {
            let dow = d.getDay();
            return dow === 0 || dow === 6;
        };
        // 仅取日期部分（当天00:00）用于比较
        let dateOnly = (d) => new Date(d.getFullYear(), d.getMonth(), d.getDate());

        if (days > 0) {
            // 先逐天对齐到周一（最多 6 天），期间消耗工作日
            while (startMyDate.getDay() !== 1 && days > 0) {
                startMyDate.setDate(startMyDate.getDate() + 1);
                if (isWeekend(startMyDate)) continue;
                if (list.has(startMyDate.toISOString().split('T')[0])) continue;
                days--;
            }
            if (days === 0) return Operand.Create(new MyDate(startMyDate));

            // 整周粗跳：起点已是周一，每 5 个工作日 = 7 天
            let afterJump = new Date(startMyDate.getTime());
            afterJump.setDate(afterJump.getDate() + Math.floor(days / 5) * 7);
            let extra = 0;
            let s0 = dateOnly(startMyDate);
            let j0 = dateOnly(afterJump);
            for (let h of list) {
                let hDate = new Date(h + 'T00:00:00');
                if (dateOnly(hDate) > s0 && dateOnly(hDate) <= j0 && !isWeekend(hDate)) {
                    extra++;
                }
            }
            startMyDate = afterJump;
            days = (days % 5) + extra;
            while (days > 0) {
                startMyDate.setDate(startMyDate.getDate() + 1);
                if (isWeekend(startMyDate)) continue;
                if (list.has(startMyDate.toISOString().split('T')[0])) continue;
                days--;
            }
        } else if (days < 0) {
            // 先逐天对齐到周五（最多 6 天），期间消耗工作日
            while (startMyDate.getDay() !== 5 && days < 0) {
                startMyDate.setDate(startMyDate.getDate() - 1);
                if (isWeekend(startMyDate)) continue;
                if (list.has(startMyDate.toISOString().split('T')[0])) continue;
                days++;
            }
            if (days === 0) return Operand.Create(new MyDate(startMyDate));

            // 整周粗跳：起点已是周五，每 5 个工作日 = 7 天
            let afterJump = new Date(startMyDate.getTime());
            afterJump.setDate(afterJump.getDate() + Math.floor(-days / 5) * -7);
            let extra = 0;
            let s0 = dateOnly(startMyDate);
            let j0 = dateOnly(afterJump);
            for (let h of list) {
                let hDate = new Date(h + 'T00:00:00');
                if (dateOnly(hDate) >= j0 && dateOnly(hDate) < s0 && !isWeekend(hDate)) {
                    extra++;
                }
            }
            startMyDate = afterJump;
            days = -((-days) % 5) - extra;
            while (days < 0) {
                startMyDate.setDate(startMyDate.getDate() - 1);
                if (isWeekend(startMyDate)) continue;
                if (list.has(startMyDate.toISOString().split('T')[0])) continue;
                days++;
            }
        }
        return Operand.Create(new MyDate(startMyDate));
    }
}

export { Function_WORKDAY };

