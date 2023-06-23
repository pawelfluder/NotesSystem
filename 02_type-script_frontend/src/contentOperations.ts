type item =
{
    Name: string
    Content: string[]
}

type tuple =
{
    type: string
    level: number
    line: string
}

export function TranslateLine(line: string)
{
    var newLine = line.replace("\t", '&nbsp;&nbsp;&nbsp;&nbsp;')
    return newLine
}

export function addTextLine1(list: HTMLUListElement | null, line: string)
{
    const newLine = TranslateLine(line)
    const item = document.createElement("li");
    const label = document.createElement("label")
    const checkbox = document.createElement("input")

    checkbox.type = "checkbox"
    label.append(newLine)
    item.append(label)
    list?.append(item)
}

export function addTextLine2(mainDiv: HTMLUListElement | null, line: string)
{
    const newLine = TranslateLine(line)
    const p = document.createElement("p");
    p.textContent = newLine
    mainDiv?.append(p)
}

export function addTextLine3(mainDiv: HTMLDivElement | null, line: string)
{
    const newLine = TranslateLine(line)
    if (mainDiv != null)
    {
        mainDiv.textContent += line
    }
}

// get all rows in the first table body

  // attach a click handler on each row

export function removeAllRows(table: HTMLTableElement | null)
{
    if (table == null)
    {
        return
    }

    const rows = table.rows;

    Array.from(rows).forEach((row, idx) =>
    {
        table.deleteRow(0)
    })
}

export function getAll4(contentTable: HTMLTableElement | null, item: item)
{
    item.Content.slice(5).forEach(x => getLine4(contentTable, x))
}



export function getLine4(table: HTMLTableElement | null, line: string)
{
    const sep1 = "&nbsp;"
    const sep2 = "  "
    const sep3 = "\t"
    const sep4 = "\u00A0"
    const tr = document.createElement("tr")
    //tr.style.border = "1px solid red"
    const td1 = document.createElement("td")
    //td1.style.border = "none"
    const td2 = document.createElement("td")
    td2.style.border = "1px solid red"
    const checkbox = document.createElement("input")
    checkbox.type = "checkbox"
    td1.append(checkbox)
    td2.textContent = sep4 + line
    tr.append(td1)
    tr.append(td2)
    table?.append(tr)
}

export function getAll5(contentTable: HTMLTableElement | null, item: item)
{
    let tuplesList = item.Content.slice(4).map(lineToTuple)
    let levelsList = tuplesList.map(x => x.level)
    const max = Math.max(...levelsList)

    addName(contentTable, item.Name, max)
    tuplesList.forEach(x => getLine5(contentTable, x, max))
}

export function addName(table: HTMLTableElement | null, name: string, max: number)
{
    const tr = document.createElement("tr")
    let td = document.createElement("td")
    tr.append(td)
    td.style.height = "20px"
    td.colSpan = max
    td.textContent = name

    table
    table?.append(tr)
}

export function getLine5(table: HTMLTableElement | null, tuple: tuple, max: number)
{
    const tr = document.createElement("tr")
    tr.style.maxWidth = "200px"
    if (tuple.type == "Header")
    {
        addCells(tr, tuple, max)
    }

    if (tuple.type == "Line")
    {
        addCells(tr, tuple, max)
    }
    table?.append(tr)
}

function addCells(row: HTMLTableRowElement, tuple: tuple, max: number)
{
    var setting: { [name: string] : string; } = {};

    let currentUrl = window.location.href;

    // all
    setting["fontSize"] = "10px"
    setting["tableMaxWidth"] = "400px"
    setting["cellHeight"] = "11px"

    // not empty cell
    setting["notEmptyCellBorder"] = "1px solid blue"

    // empty cell
    setting["emptyCellBorder"] = "1px solid white"
    setting["emptyCellWidth"] = "7px"

    // line
    setting["leftBorder"] = "1px solid black"

    for (let i = 0; i < max + 1; i++)
    {
        let td = document.createElement("td")
        td.style.height = setting["cellHeight"]
        
        row.append(td)

        if (tuple.type == "Header")
        {
            //td.style.border = setting["notEmptyCellBorder"]
        }

        if (tuple.type == "Line")
        {
            //td.style.width = setting["emptyCellWidth"]
        }

        // empty cell
        if (i < tuple.level - 1)
        {
            //td.style.border = setting["emptyCellBorder"]
            td.style.width = setting["emptyCellWidth"]
        }
        
        if (i == tuple.level - 1)
        {
            const span = (max - tuple.level + 1)
            td.colSpan = span

            if (tuple.type == "Header")
            {
                td.style.border = setting["notEmptyCellBorder"]
            }

            if (tuple.type == "Line")
            {
                td.style.borderLeft = setting["leftBorder"]
            }
            
            td.style.maxWidth = setting["tableMaxWidth"]
            td.textContent = tuple.line
            td.style.fontSize = setting["fontSize"]
            
            break;
        }        
    }
}

function lineToTuple(line2: string) : tuple
{
    const level = getLevel(line2)
    const type = getType(line2)
    const line = getLine(line2)
    const tuple = {type, level, line}
    return tuple
}

function getLine(line: string) : string
{
    let line2 = line.trimStart()
    if (line2.charAt(0) == '/' &&
        line2.charAt(1) == '/')
    {
        line2 = line2.substring(2)
    }
    return line2
}

function getType(line: string) : string
{
    const line2 = line.trimStart()
    if (line2.charAt(0) == '/' &&
        line2.charAt(1) == '/')
    {
        return "Header"
    }

    return "Line"
}

function getLevel(line: string) : number
{
    let level = 1
    let tmp = line

    for (let i = 0; i < line.length; i++)
    {
        if (tmp.charAt(i) !== '\t')
        {
            break;
        }
        level++        
    }
    return level
}

function getLevel2(line: string) : number
{
    let level = 1
    let tmp = line

    while(line.charAt(0) === '/t')
    {
        line = line.substring(1);
        level++
    }
    return level
}

function selectAll()
{
    const all = document.querySelectorAll('*')
    all.forEach(x =>
    {
        console.log("elem: " + x)
    })
}
