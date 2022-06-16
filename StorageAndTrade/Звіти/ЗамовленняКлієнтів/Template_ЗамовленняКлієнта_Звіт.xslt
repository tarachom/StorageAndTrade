<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="html" indent="yes"/>

    <xsl:template match="/root">

		<table border="1" width="100%">

			<tr>
				<th>Номенклатура</th>
				<th>Характеристика номенклатури</th>
				<th>Замовлено</th>
				<th>Сума</th>
			</tr>

			<xsl:for-each select="row">

				<tr>
					<td>
						<xsl:value-of select="Номенклатура_Назва"/>
					</td>
					<td>
						<xsl:value-of select="ХарактеристикаНоменклатури_Назва"/>
					</td>
					<td>
						<xsl:value-of select="Замовлено"/>
					</td>
					<td>
						<xsl:value-of select="Сума"/>
					</td>
				</tr>
				
			</xsl:for-each>
			
		</table>
		
    </xsl:template>
	
</xsl:stylesheet>
