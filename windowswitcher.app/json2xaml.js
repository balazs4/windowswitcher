const [_, __, theme] = process.argv;
const fs = require("fs");
const input = require(`${theme}`);

const colors = Object.keys(input.colors).map(
  key => `<Color x:Key="${key}">${input.colors[key]}</Color>
   <SolidColorBrush x:Key="${key}.brush" Color="{StaticResource ${key}}"/>
   `
);


const tokenColors = input.tokenColors
.filter(x => x.name === 'Keyword')
.map(x => `
<Color x:Key="${x.name}">${x.settings.foreground}</Color>
<SolidColorBrush x:Key="${x.name}.brush" Color="{StaticResource ${x.name}}"/>
`)

fs.writeFileSync(
  `${theme.replace('.json','')}.xaml`,
  `<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
xmlns:local="clr-namespace:windowswitcher.app">
    ${colors.join("\n")}
    ${tokenColors.join("\n")}
</ResourceDictionary>
`
);
